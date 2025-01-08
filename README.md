# Appointment Management API

A RESTful API for managing patient appointments in a healthcare clinic. This API includes authentication to ensure that only authorized users can access or modify data.

## Overview

The Appointment Management API provides a secure and efficient way to handle patient appointments in a healthcare setting. It implements JWT-based authentication and offers comprehensive CRUD operations for appointment management.

## Features

* **User Authentication**
  * Register new users with secure credential management
  * Login functionality with JWT token generation

* **Appointment Management**
  * Full CRUD operations for appointments
  * Secure access control using JWT authentication

* **Database Integration**
  * Built using Entity Framework Core
  * MSSQL database backend for reliable data storage

## Technical Requirements

* .NET Core SDK
* SQL Server
* API testing tool (Swagger or postman)

## Setup Guide

### 1. Repository Setup

```bash
git clone <repository_url>
cd <repository_folder>
```

### 2. Database Configuration

Update `appsettings.json` with your database connection:

```json
{
   "ConnectionStrings": {
   "DefaultConnection": "Data Source=DESKTOP-41QOLKC;Initial Catalog=DoctorAppointment;Trusted_Connection=true;TrustServerCertificate=true;"
 }
}
```

### 3. Database Migration

Execute the following commands:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 4. Launch Application

```bash
dotnet run
```

The API will be accessible at:
* HTTPS: `https://localhost:5001`
* HTTP: `http://localhost:5000`

## API Documentation

### Authentication Endpoints

#### Register User
* **Method:** POST
* **Endpoint:** `/register`
* **Request Body:**
  ```json
  {
    "username": "your_username",
    "password": "your_password"
  }
  ```
* **Response:**
  ```json
  {
    "message": "Registration Successful"
  }
  ```

#### User Login
* **Method:** POST
* **Endpoint:** `/login`
* **Request Body:**
  ```json
  {
    "username": "your_username",
    "password": "your_password"
  }
  ```
* **Response:**
  ```json
  {
    "token": "your_jwt_token"
  }
  ```

### Appointment Endpoints

**Note:** All appointment endpoints require JWT authentication via the `Authorization` header:
```
Authorization: Bearer <your_jwt_token>
```

#### Create Appointment
* **Method:** POST
* **Endpoint:** `/appointments`
* **Request Body:**
  ```json
  {
    "patientName": "John Doe",
    "patientContactInfo": "1234567890",
    "appointmentDateTime": "2025-01-10T14:00:00",
    "doctorId": 1
  }
  ```
* **Response:**
  ```json
  {
    "message": "Appointment created successfully"
  }
  ```

#### Retrieve All Appointments
* **Method:** GET
* **Endpoint:** `/appointments`
* **Response:**
  ```json
  [
    {
      "appointmentId": 1,
      "patientName": "John Doe",
      "patientContactInfo": "1234567890",
      "appointmentDateTime": "2025-01-10T14:00:00",
      "doctorId": 1
    }
  ]
  ```

#### Retrieve Single Appointment
* **Method:** GET
* **Endpoint:** `/appointments/{id}`
* **Response:**
  ```json
  {
    "appointmentId": 1,
    "patientName": "John Doe",
    "patientContactInfo": "1234567890",
    "appointmentDateTime": "2025-01-10T14:00:00",
    "doctorId": 1
  }
  ```

#### Update Appointment
* **Method:** PUT
* **Endpoint:** `/appointments/{id}`
* **Request Body:**
  ```json
  {
    "patientName": "John Doe Updated",
    "patientContactInfo": "9876543210",
    "appointmentDateTime": "2025-01-11T15:00:00",
    "doctorId": 2
  }
  ```
* **Response:**
  ```json
  {
    "message": "Appointment updated successfully"
  }
  ```

#### Delete Appointment
* **Method:** DELETE
* **Endpoint:** `/appointments/{id}`
* **Response:**
  ```json
  {
    "message": "Appointment deleted successfully"
  }
  ```
