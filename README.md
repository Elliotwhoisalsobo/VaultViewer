# VaultViewer

## Goal
Developed to solve the issue where sensitive data had to be manually requested from multiple managers, due to the absence of a secure central storage solution caused by strict security policies.

## Description
A user-friendly, offline desktop application built with **WPF**, serving as a central hub for storing and retrieving sensitive data — without the need for an internet connection or IT expertise.

## Features
- User panel and admin panel with clear access rights and role management.  
- Designed for both technical and non-technical users to operate efficiently.  

## Architecture
- Based on a **Layered Architecture pattern** for scalability and maintainability:  
  - Business Logic  
  - Data Access  
  - Service  
  - UI  

## Security
- Secure user authentication with **bcrypt** password hashing.  
- All sensitive data stored with a strong focus on integrity and confidentiality.  

## Database Integration
- Connects to a **MySQL database** for safe storage and retrieval of user data and records.  

## Tech Stack
- **C#**  
- **WPF**  
- **MySQL**  
- **Design Patterns (Layered Architecture)**  
