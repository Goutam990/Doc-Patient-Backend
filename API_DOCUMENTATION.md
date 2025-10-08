# API Documentation: Appointments

This document provides details on the API endpoints for managing appointments.

---

## 1. Book an Appointment

This endpoint allows a patient to book a new appointment with a doctor.

- **Method:** `POST`
- **URL:** `/api/appointments/book`
- **Description:** Creates a new appointment.
- **Authorization:** Requires an authenticated user with the `Patient` role. The user's JWT token must be included in the `Authorization` header.

### Request Body

The request body must be a JSON object with the following properties:

| Field           | Type    | Description                               | Required |
| --------------- | ------- | ----------------------------------------- | -------- |
| `age`           | integer | The age of the patient.                   | No       |
| `gender`        | string  | The gender of the patient.                | No       |
| `startTime`     | string  | The start time of the appointment (UTC).  | Yes      |
| `phoneNumber`   | string  | The patient's phone number.               | Yes      |
| `address`       | string  | The patient's address.                    | No       |
| `doctorId`      | string  | The ID of the doctor for the appointment. | Yes      |
| `paymentIntentId` | string  | The ID of the payment intent from Stripe. | Yes      |

**Example Request:**

```json
{
  "age": 30,
  "gender": "Male",
  "startTime": "2024-10-27T10:00:00Z",
  "phoneNumber": "123-456-7890",
  "address": "123 Main St, Anytown, USA",
  "doctorId": "a1b2c3d4-e5f6-g7h8-i9j0-k1l2m3n4o5p6",
  "paymentIntentId": "pi_1J2b3c4d5e6f7g8h9i0j"
}
```

### Success Response

- **Status Code:** `201 Created`
- **Content:** A JSON object representing the newly created appointment.

**Example Response:**

```json
{
  "id": 1,
  "patientName": "John Doe",
  "age": 30,
  "gender": "Male",
  "startTime": "2024-10-27T10:00:00Z",
  "endTime": "2024-10-27T11:00:00Z",
  "phoneNumber": "123-456-7890",
  "address": "123 Main St, Anytown, USA",
  "status": "Pending",
  "patientId": "p1q2r3s4-t5u6-v7w8-x9y0-z1a2b3c4d5e6",
  "doctorId": "a1b2c3d4-e5f6-g7h8-i9j0-k1l2m3n4o5p6"
}
```

### Error Responses

- **Status Code:** `400 Bad Request`
  - **Reason:** Validation failed (e.g., missing required fields, invalid date format, date in the past, doctor not found, slot not available, overlapping appointments).
  - **Response Body:**
    ```json
    {
      "message": "Error message describing the issue."
    }
    ```
- **Status Code:** `401 Unauthorized`
  - **Reason:** The user is not authenticated.
- **Status Code:** `403 Forbidden`
  - **Reason:** The user does not have the `Patient` role.
- **Status Code:** `500 Internal Server Error`
  - **Reason:** An unexpected error occurred on the server.

---

## 2. Get Upcoming Appointments for a Patient

This endpoint retrieves a list of all upcoming appointments for a specific patient.

- **Method:** `GET`
- **URL:** `/api/appointments/patient/{patientId}`
- **Description:** Retrieves all upcoming appointments for the specified patient.
- **Authorization:** Requires an authenticated user with either the `Admin` or `Patient` role.

### URL Parameters

| Parameter   | Type   | Description                       | Required |
| ----------- | ------ | --------------------------------- | -------- |
| `patientId` | string | The ID of the patient.            | Yes      |

### Success Response

- **Status Code:** `200 OK`
- **Content:** A JSON array of appointment objects.

**Example Response:**

```json
[
  {
    "id": 1,
    "patientName": "John Doe",
    "age": 30,
    "gender": "Male",
    "startTime": "2024-10-27T10:00:00Z",
    "endTime": "2024-10-27T11:00:00Z",
    "phoneNumber": "123-456-7890",
    "address": "123 Main St, Anytown, USA",
    "status": "Pending",
    "patientId": "p1q2r3s4-t5u6-v7w8-x9y0-z1a2b3c4d5e6",
    "doctorId": "a1b2c3d4-e5f6-g7h8-i9j0-k1l2m3n4o5p6"
  }
]
```

### Error Responses

- **Status Code:** `401 Unauthorized`
  - **Reason:** The user is not authenticated.
- **Status Code:** `403 Forbidden`
  - **Reason:** The user does not have the `Admin` or `Patient` role.
- **Status Code:** `500 Internal Server Error`
  - **Reason:** An unexpected error occurred on the server.
