# VaultViewer

## Goal
Developed to solve the issue where sensitive data had to be manually requested from multiple managers, due to the absence of a secure central storage solution caused by strict security policies.

## Description
A user-friendly, offline desktop application built with **WPF**, serving as a central hub for storing and retrieving sensitive data — without the need for an internet connection or IT expertise.

## Features
- User panel and admin panel with clear access rights and role management.  
- Designed for both technical and non-technical users to operate efficiently.
- Filter & sort data
- Export data to CSV/Excel in one click

## Architecture
- Based on a **Layered Architecture pattern** for scalability and maintainability:  
  - Business Logic  
  - Data Access  
  - Service  
  - UI  

## Security
- Secure user authentication with **password hashing**.  
- All sensitive data stored with a strong focus on integrity and confidentiality.  

## Database Integration
- Connects to a **MySQL database** for safe storage and retrieval of user data and records.  

## Tech Stack
- **C#**  
- **WPF**  
- **MySQL**  
- **Design Patterns (Layered Architecture)**

## Pictures
WIP

## To add
- When admin creates a user it makes a one time use password instead of making the user inactive 
- fix bug when adding new password --> password must be X chars (it shows x instead of number)
