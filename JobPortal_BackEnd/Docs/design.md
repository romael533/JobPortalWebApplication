# Job Portal: design note

Job Portal lets employers post jobs and candidates search and apply. It has three roles: Candidate, Employer and Admin. The stack is ASP.NET Core Web API, React, PostgreSQL, Docker Compose and Git.

Supporting diagrams and flows: [docs/Job_Portal_flow_and_sequence_diagrams.pdf](Job_Portal_flow_and_sequence_diagrams.pdf)

## 1. Architecture

![Architecture](images/architecture.png)

Each request passes the middleware first (authentication only), then the controller (role attribute), the service (business rules and ownership checks), and EF Core. The response returns the same way.

## 2. Entity-relationship diagram

![Entity-relationship diagram](images/entity-relationship.png)

- One role per user, stored in the Identity role tables.
- Each user has at most one profile: a CandidateProfile or an EmployerProfile.
- One employer owns at most one Company. A Company posts many Jobs.
- A Job receives many JobApplications. One candidate can apply to a job once (unique CandidateUserId and JobId).

## 3. API endpoints

All routes start with `/api/v1`.

| # | Method and route | Who | Purpose | Request fields | Success response |
|---|---|---|---|---|---|
| 1 | POST /api/v1/auth/register | Public | Create a Candidate or Employer account | Email, Password, Name, Phone Number, Role, Active Status | User id |
| 2 | POST /api/v1/auth/login | Public | Check login details and issue a JWT | Email, Password | JWT token |
| 3 | GET /api/v1/companies/mine | Employer | Show the employer's company | none | Company Name, Location, Website, Description |
| 4 | POST /api/v1/companies | Employer | Create the employer's one company | Company Name, Location, Website, Description | Company id |
| 5 | GET /api/v1/jobs/mine | Employer | List the jobs this employer posted | none | Title, Company, Job Type, Skills, Status |
| 6 | POST /api/v1/jobs | Employer | Post a new open job under the company | Title, Location, Job Type, Description, Skills, Experience Level | Job id |
| 7 | GET /api/v1/jobs/{id} | Public | Show one job's details | id in URL | Title, Location, Job Type, Status, Company, Description, Skills, Experience Level |
| 8 | GET /api/v1/jobs/{id}/candidates | Employer (own job) | List candidates who applied to one job | id in URL | Candidate Name, Contact Email, Contact Number, Experience Level, Skills, Address, Language known, Status, Application Id, Applied Date |
| 9 | PUT /api/v1/jobs/{id} | Employer (own job) | Change the details of one job | Title, Location, Job Type, Status, Description, Skills, Experience Level | Success message |
| 10 | POST /api/v1/jobs/{id}/close | Employer (own job) | Close a job so it takes no new applications | id in URL | Success message |
| 11 | PUT /api/v1/applications/{id}/status | Employer (own job) | Move an application to the next allowed status | Status | Success message |
| 12 | POST /api/v1/candidates | Candidate | Create the candidate's profile | Candidate Name, Contact Email, Contact Number, Experience Level, Skills, Address, Language known | User id |
| 13 | GET /api/v1/candidates/mine | Candidate | Show the candidate's own profile | none | Candidate Name, Contact Email, Contact Number, Experience Level, Skills, Address, Language known |
| 14 | PUT /api/v1/candidates/mine | Candidate | Update the candidate's own profile | Candidate Name, Contact Email, Contact Number, Experience Level, Skills, Address, Language known | Success message |
| 15 | GET /api/v1/jobs | Public | List open jobs for browsing | none | Title, Company, Job Type, Skills, Status |
| 16 | GET /api/v1/jobs?keyword&location&jobType&page&pageSize | Public | Search open jobs with filters and paging | keyword, location, jobType, page, pageSize | Total count, page number, items |
| 17 | POST /api/v1/jobs/{id}/applications | Candidate | Apply to an open job, once | id in URL | Application Id, Applied Date |
| 18 | GET /api/v1/applications/mine | Candidate | List my applications and their status | none | Title, Company, Job Type, Skills, Status, Application Id, Applied Date |
| 19 | POST /api/v1/applications/{id}/withdraw | Candidate (own) | Withdraw an eligible application | id in URL | Success message |
| 20 | GET /api/v1/admin/companies | Admin | List all companies with their employer and applicant count | none | Company Name, Location, Website, Description, Employer Name, Applied Candidates |
| 21 | GET /api/v1/admin/users | Admin | List employer accounts | none | Name, Contact Email, Contact Phone |
| 22 | GET /api/v1/admin/candidates | Admin | List candidate accounts | none | Name, Contact Email, Contact Phone |
| 23 | POST /api/v1/admin/users/{employerId}/deactivate | Admin | Deactivate an employer so they cannot log in | id in URL | Success message |
| 24 | POST /api/v1/admin/candidates/{candidateId}/deactivate | Admin | Deactivate a candidate so they cannot log in | id in URL | Success message |
| 25 | POST /api/v1/admin/jobs/{id}/close | Admin | Close any job, whoever owns it | id in URL | Success message |

**Open decisions:** job type values (Full-time or FullTime); candidate create and update as one endpoint or two; merge endpoints 15 and 16; rename admin users to employers.

## 4. Work split into pull requests

Branches: `main` (release), `develop` (integration), and one branch per pull request, merged into `develop` in this order. Each pull request is reviewed, and comments are answered before merge.

| # | Branch | Contents | Depends on |
|---|---|---|---|
| 1 | chore/setup | Repo, main and develop, backend and frontend skeletons, docker-compose with PostgreSQL, README stub | none |
| 2 | feature/auth | Identity setup, EF Core migration, IsActive flag, roles seeded, register and login, JWT with sub, email and role claims, JWT middleware, role attributes, admin seeded from settings, 401 and 403 tests | 1 |
| 3 | feature/companies-and-jobs | Company table, create and view my company, Job table, post, update, close, my jobs, job details, list and search open jobs with paging, ownership tests | 2 |
| 4 | feature/candidates-and-applications | CandidateProfile table, create, view and update my profile, apply with duplicate and closed-job checks, my applications, withdraw, employer lists applicants and updates status, transition rules, 409 tests | 3 |
| 5 | feature/admin | List users, companies, jobs and candidates; deactivate user; close any job | 4 |
| 6 | feature/frontend | Login and register pages, API helper with bearer token, protected routes, 401 handling, component test; job list, search, details, apply, my applications, candidate profile, employer and admin screens, API mock test | 2 to 5 |
| 7 | chore/final-checks | README with seeded accounts and test steps, final test run, docker compose up check | 6 |

**Release:** after pull request 7, `develop` is merged into `main`. No commits after the end of Fri 4 Dec.

## 5. Authentication approach

ASP.NET Core Identity stores users, hashes passwords and holds roles, through EF Core and PostgreSQL. The API signs a JWT at login and checks it on every protected call with the JwtBearer middleware. Identity's built-in pages are not used.

**JWT settings**

| Setting | Meaning | Where it is kept |
|---|---|---|
| Jwt:Key | Secret used to sign the token. At least 32 characters. | dotnet user-secrets locally; `.env` for Docker. Never committed. |
| Jwt:Issuer | Who created the token (the API). Checked on every request. | Configuration |
| Jwt:Audience | Who the token is for (the Job Portal). Checked on every request. | Configuration |
| Lifetime | 30 to 60 minutes. No refresh tokens: the user logs in again. | Configuration |

**Claims in the token**

| Claim | Value | Used for |
|---|---|---|
| sub | User id from Identity | Ownership checks in the service layer. Never taken from the request. |
| email | User email | Display only |
| role | Candidate, Employer or Admin | Role checks on controllers. The claim name "role" is the same in both places. |

**How the token is sent:** React sends `Authorization: Bearer <token>` on every protected call. One API helper adds it, so no component does it by hand.

**401 and 403 rules**

| Code | When | Example |
|---|---|---|
| 401 | Not logged in: no token, an expired token, or a bad signature. Checked in middleware, before any controller runs. | No Authorization header. Login with an unknown email or wrong password (same message for both). |
| 403 | Logged in but not allowed: wrong role, or data owned by another user. | Candidate calling an employer endpoint. Employer asking for another employer's applications. |

**Known trade-offs:** a deactivated user keeps access until the current token expires (up to 60 minutes). The token is kept in localStorage, which is acceptable here but exposed to scripts; production would use an HttpOnly cookie. Plain HTTP is used on localhost only; a real deployment needs HTTPS.

## 6. Error format, paging and sorting

Every error returns a ProblemDetails body:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.10",
  "title": "Job is closed",
  "status": 409,
  "detail": "This job no longer accepts applications."
}
```

| Code | Meaning | Used in this app for |
|---|---|---|
| 400 | Invalid input | Missing or bad fields, invalid job type or status value, page size outside 1 to 50 |
| 401 | Not logged in | No token, expired token, bad signature, or wrong login details |
| 403 | Logged in, not allowed | Wrong role, data owned by another user, or a deactivated user's login |
| 404 | Not found | Job, application, company or user id does not exist; a closed job shown to the public |
| 409 | Conflict | Email already used, company exists, duplicate application, job closed, invalid status change, already closed or inactive |

**Paging and sorting**

| Setting | Value |
|---|---|
| page | Starts at 1 |
| pageSize | Default 10, maximum 50 (above 50 returns 400) |
| Sort | Newest first, for job search and the admin lists |
| Response | Items, page, pageSize and total count |

## 7. Seed data, Docker, frontend and testing

**Seed data (created at startup in development)**

| Account or data | Details |
|---|---|
| Employer | One employer with a company (for example, Sample Tech). Email and password come from the seed settings. |
| Candidate | One candidate with a profile. Email and password from the seed settings. |
| Admin | Created from Admin email and password in settings. Not registrable. |
| Jobs | At least five jobs under the seeded company, mix of Open and one Closed. |

**Docker layout** (ports are proposed)

| Service | Port | Settings from `.env` |
|---|---|---|
| frontend (React) | 3000 | API base address |
| api (.NET) | 5000 | Connection string, Jwt key, issuer, audience, lifetime, admin email and password |
| db (PostgreSQL) | 5432, internal to Compose | Database name, user and password |

**Frontend plan**

- Pages: login, register, job listing, job details, job search and filters, candidate profile, apply, my applications, employer dashboard, job management, application management, and admin screens.
- Protected routes: candidate, employer and admin pages each need the matching role. The check improves the experience only; the API enforces the rules.
- API helper: one module adds the Authorization header to every call. On a 401 it clears the token and goes to login.

**Testing plan**

| Type | Cases |
|---|---|
| Backend unit tests | Successful job creation, successful application, duplicate application, application to a closed job, unauthorized access, application status update |
| Security tests (required) | No token gives 401; candidate on an employer endpoint gives 403; employer asking for another employer's applications is refused; candidate changing another candidate's profile is refused |
| Frontend component test | One component, for example the login form, with validation shown |
| API mock test | One screen that loads jobs from a mocked API, with loading and error states |

## 8. Flow and sequence diagrams

Each feature has a flow diagram (the checks and their failure responses) and a sequence diagram (the calls between the layers). The full set is also in the PDF: [Job_Portal_flow_and_sequence_diagrams.pdf](Job_Portal_flow_and_sequence_diagrams.pdf).

### Register

**POST /api/v1/auth/register**

```json
{
  "email": "string",
  "password": "string",
  "name": "string",
  "phoneNumber": "string",
  "role": "Candidate | Employer"
}
```

![Register flow](images/flows/01-register-flow.png)

![Register sequence](images/flows/01-register-sequence.png)

### Sign in

**POST /api/v1/auth/login**

```json
{
  "email": "string",
  "password": "string"
}
```

![Sign in flow](images/flows/02-sign-in-flow.png)

![Sign in sequence](images/flows/02-sign-in-sequence.png)

### Add company

**POST /api/v1/companies**

```json
{
  "name": "string",
  "location": "string",
  "website": "string",
  "description": "string"
}
```

![Add company flow](images/flows/03-add-company-flow.png)

![Add company sequence](images/flows/03-add-company-sequence.png)

### Post job

**POST /api/v1/jobs**

```json
{
  "title": "string",
  "location": "string",
  "jobType": "Full-time | Part-time | Contract",
  "description": "string",
  "skills": ["string"],
  "experienceLevel": "string"
}
```

![Post job flow](images/flows/04-post-job-flow.png)

![Post job sequence](images/flows/04-post-job-sequence.png)

### Update job

**PUT /api/v1/jobs/{id}**

```json
{
  "title": "string",
  "location": "string",
  "jobType": "Full-time | Part-time | Contract",
  "description": "string",
  "skills": ["string"],
  "experienceLevel": "string"
}
```

![Update job flow](images/flows/05-update-job-flow.png)

![Update job sequence](images/flows/05-update-job-sequence.png)

### Close job

**POST /api/v1/jobs/{id}/close**

No request body. The id is in the URL.

![Close job flow](images/flows/06-close-job-flow.png)

![Close job sequence](images/flows/06-close-job-sequence.png)

### Search and filter jobs

**GET /api/v1/jobs?keyword=string&location=string&jobType=string&page=int&pageSize=int**

No request body. The id is in the URL.

![Search and filter jobs flow](images/flows/07-search-and-filter-jobs-flow.png)

![Search and filter jobs sequence](images/flows/07-search-and-filter-jobs-sequence.png)

### Apply for job

**POST /api/v1/jobs/{id}/applications**

No request body. The id is in the URL.

![Apply for job flow](images/flows/08-apply-for-job-flow.png)

![Apply for job sequence](images/flows/08-apply-for-job-sequence.png)

### Withdraw application

**POST /api/v1/applications/{id}/withdraw**

No request body. The id is in the URL.

![Withdraw application flow](images/flows/09-withdraw-application-flow.png)

![Withdraw application sequence](images/flows/09-withdraw-application-sequence.png)

### Update application status

**PUT /api/v1/applications/{id}/status**

```json
{
  "status": "UnderReview | Shortlisted | Rejected | Hired"
}
```

![Update application status flow](images/flows/10-update-application-status-flow.png)

![Update application status sequence](images/flows/10-update-application-status-sequence.png)

### Save candidate profile

**POST /api/v1/candidates** (create) or **PUT /api/v1/candidates/mine** (update)

**
```json
{
  "name": "string",
  "email": "string",
  "phone": "string",
  "experienceLevel": "string",
  "skills": ["string"],
  "address": "string",
  "languagesKnown": ["string"]
}
```

![Save candidate profile flow](images/flows/11-save-candidate-profile-flow.png)

![Save candidate profile sequence](images/flows/11-save-candidate-profile-sequence.png)

### Admin deactivate employer

**POST /api/v1/admin/users/{employerId}/deactivate**

No request body. The id is in the URL.

![Admin deactivate employer flow](images/flows/12-admin-deactivate-employer-flow.png)

![Admin deactivate employer sequence](images/flows/12-admin-deactivate-employer-sequence.png)

### Admin deactivate candidate

**POST /api/v1/admin/candidates/{candidateId}/deactivate**

No request body. The id is in the URL.

![Admin deactivate candidate flow](images/flows/13-admin-deactivate-candidate-flow.png)

![Admin deactivate candidate sequence](images/flows/13-admin-deactivate-candidate-sequence.png)

### Admin close job

**POST /api/v1/admin/jobs/{id}/close**

No request body. The id is in the URL.

![Admin close job flow](images/flows/14-admin-close-job-flow.png)

![Admin close job sequence](images/flows/14-admin-close-job-sequence.png)

### Admin list companies

**GET /api/v1/admin/companies**

No request body. The id is in the URL.

![Admin list companies flow](images/flows/15-admin-list-companies-flow.png)

![Admin list companies sequence](images/flows/15-admin-list-companies-sequence.png)