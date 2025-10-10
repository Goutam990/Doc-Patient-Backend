# Patient Appointment Booking System - API Documentation

This document provides a complete reference for the backend API of the Patient Appointment Booking System.

---

## 1. Authentication (`/api/auth`)

These endpoints handle patient registration and login.

### Register a New Patient

-   **Endpoint:** `POST /api/auth/register`
-   **Description:** Creates a new `AspNetUser` and an associated `PatientInfo` record.
-   **Authorization:** Public.

**Request Body (`RegisterDto`):**

```json
{
  "email": "patient@example.com",
  "password": "Password@123",
  "firstName": "John",
  "lastName": "Doe",
  "dateOfBirth": "1990-01-15T00:00:00Z",
  "gender": "Male",
  "countryCode": "+91",
  "phoneNumber": "9876543210",
  "illnessHistory": "None",
  "picture": "http://example.com/profile.jpg"
}
```

**Success Response (200 OK):**

```json
{
  "message": "User registered successfully."
}
```

**Error Response (400 Bad Request):**

```json
{
  "message": "An account with this email already exists."
}
```

### Login

-   **Endpoint:** `POST /api/auth/login`
-   **Description:** Authenticates a user (Patient or Doctor) and returns a JWT.
-   **Authorization:** Public.

**Request Body (`LoginDto`):**

```json
{
  "email": "patient@example.com",
  "password": "Password@123"
}
```

**Success Response (200 OK):**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Error Response (401 Unauthorized):**

```json
{
  "message": "Invalid email or password."
}
```

---

### Get Current User

-   **Endpoint:** `GET /api/auth/me`
-   **Description:** Returns the authenticated user's basic information (id, email, roles) derived from the JWT claims. Useful for frontend to bootstrap user state after login or page reload.
-   **Authorization:** Requires Bearer JWT in `Authorization` header.

**Success Response (200 OK):**

```json
{
  "id": "user-id-guid",
  "email": "user@example.com",
  "roles": ["Patient"]
}
```

**Notes for Frontend**

- Set `VITE_API_URL` in the frontend `.env` (or use `.env.local`) to point to the backend, e.g. `http://localhost:5000` or `https://localhost:7241` depending on your launch settings.
- The frontend `src/services/api.js` automatically attaches token from `localStorage` to requests.

## 2. Doctor APIs (`/api/doctor`)

These endpoints are for the Doctor role only.

-   **Authorization:** Requires `Doctor` role JWT.

### Get All Patients

-   **Endpoint:** `GET /api/doctor/patients`
-   **Description:** Retrieves a list of all registered patients' details.

**Success Response (200 OK):**
Returns an array of `PatientDetailsDto`.

### Get Patient Details

-   **Endpoint:** `GET /api/doctor/patients/{id}`
-   **Description:** Retrieves the details for a single patient by their `PatientInfo` ID.

**Success Response (200 OK):**
Returns a single `PatientDetailsDto`.

### Get Upcoming Appointments

-   **Endpoint:** `GET /api/doctor/appointments/upcoming`
-   **Description:** Gets a list of all pending and confirmed appointments.

**Success Response (200 OK):**
Returns an array of `AppointmentDto`.

### Update Availability

-   **Endpoint:** `POST /api/doctor/availability`
-   **Description:** Sets or updates the doctor's working hours. Triggers auto-cancellation of appointments outside the new hours.

**Request Body (`UpdateAvailabilityDto`):**

```json
{
  "startTime": "2025-11-01T09:00:00Z",
  "endTime": "2025-11-01T17:00:00Z"
}
```

### Approve/Reject/Revisit an Appointment

-   **Approve:** `POST /api/doctor/appointments/{id}/approve`
-   **Reject:** `POST /api/doctor/appointments/{id}/reject`
-   **Schedule Revisit:** `POST /api/doctor/appointments/{id}/revisit`
-   **Description:** Performs management actions on a specific appointment.

---

## 3. Patient APIs (`/api/patient`)

These endpoints are for the Patient role only.

-   **Authorization:** Requires `Patient` role JWT. The patient's user ID is automatically retrieved from the token.

### Get Patient's Appointments

-   **Upcoming:** `GET /api/patient/appointments/upcoming`
-   **Completed:** `GET /api/patient/appointments/completed`
-   **All:** `GET /api/patient/appointments/all`
-   **Description:** Retrieves lists of the authenticated patient's appointments based on status.

**Success Response (200 OK):**
Returns an array of `AppointmentDto`.

### Book a New Appointment

-   **Endpoint:** `POST /api/patient/appointments`
-   **Description:** Books a new appointment. Fails if the patient already has an active (Pending/Confirmed) appointment.

**Request Body (`CreateAppointmentDto`):**

```json
{
  "startTime": "2025-11-01T10:00:00Z"
}
```

**Success Response (201 Created):**
Returns the created `AppointmentDto`.

### Edit (Reschedule) an Appointment

-   **Endpoint:** `PUT /api/patient/appointments/{id}`
-   **Description:** Allows a patient to change the time of their own appointment before the scheduled time.

**Request Body (`UpdateAppointmentDto`):**

```json
{
  "startTime": "2025-11-01T11:00:00Z"
}
```

### Cancel an Appointment

-   **Endpoint:** `POST /api/patient/appointments/{id}/cancel`
-   **Description:** Allows a patient to cancel their own pending or confirmed appointment.

---

## 4. Payment APIs (`/api/payments`)

### Create Payment Intent

-   **Endpoint:** `POST /api/payments/create-intent/{appointmentId}`
-   **Description:** Initializes a Stripe payment intent for a given appointment.
-   **Authorization:** Requires `Patient` role JWT.

**Success Response (200 OK):**

```json
{
  "clientSecret": "pi_...",
  "paymentIntentId": "pi_..."
}
```

---

## 5. Stripe Webhook (`/api/stripe-webhook`)

-   **Endpoint:** `POST /api/stripe-webhook`
-   **Description:** A public endpoint to receive webhook events from Stripe (e.g., `payment_intent.succeeded`). This endpoint has no authorization.
