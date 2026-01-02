# OnboardingApp

### 🟢 `ResumeStatus`

| Value        | Meaning                         |
| ------------ | ------------------------------- |
| Submitted    | Resume uploaded by recruiter    |
| Under Review | Resume is being reviewed        |
| Shortlisted  | Approved for internal interview |
| Rejected     | Not suitable                    |
| On Hold      | Awaiting further clarification  |

---

### 🟢 `CtsInternalInterviewStatus`

(Internal screening before client submission)

| Value         | Meaning                    |
| ------------- | -------------------------- |
| Not Scheduled | Interview not yet planned  |
| Scheduled     | Interview scheduled        |
| Completed     | Interview completed        |
| Selected      | Cleared internal interview |
| Rejected      | Failed internal screening  |
| No Show       | Candidate did not attend   |

---

### 🟢 `ClientInterviewStatus`

| Value             | Meaning                    |
| ----------------- | -------------------------- |
| Awaiting Schedule | Client has resume          |
| Scheduled         | Client interview scheduled |
| Completed         | Interview completed        |
| Selected          | Client approved            |
| Rejected          | Client rejected            |
| On Hold           | Client paused decision     |
| Feedback Pending  | Awaiting client feedback   |

---

### 🟢 `FinalStatus`

(Overall hiring decision)

| Value          | Meaning                  |
| -------------- | ------------------------ |
| In Progress    | Hiring process ongoing   |
| Offered        | Offer released           |
| Offer Accepted | Candidate accepted       |
| Offer Rejected | Candidate declined       |
| Joined         | Candidate onboarded      |
| Dropped        | Candidate exited process |

---

### 🟢 `SecurityCheckStatus`

| Value         | Meaning                      |
| ------------- | ---------------------------- |
| Not Initiated | Background check not started |
| In Progress   | Verification ongoing         |
| Cleared       | Security check passed        |
| Failed        | Security check failed        |

---

### 🟢 `OnboardingRequestSent`

| Value     | Meaning                     |
| --------- | --------------------------- |
| 0 (false) | Onboarding request not sent |
| 1 (true)  | Onboarding initiated        |

---

## 3️⃣ Status Flow Diagram (Simplified)

```
Resume Submitted
      ↓
Internal Interview
      ↓
Client Interview
      ↓
Final Decision
      ↓
Security Check
      ↓
Onboarding


| Color            | Meaning                    |
| ---------------- | -------------------------- |
| 🟢 `bg-success`  | Success / Cleared / Joined |
| 🟡 `bg-warning`  | In Progress / Pending      |
| 🔵 `bg-info`     | Awaiting / Submitted       |
| 🔷 `bg-primary`  | Completed / Offered        |
| 🔴 `bg-danger`   | Rejected / Failed          |
| ⚫ `bg-dark`      | No show / Dropped          |
| ⚪ `bg-secondary` | Not started / On Hold      |
