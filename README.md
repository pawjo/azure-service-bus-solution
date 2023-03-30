# Azure Service bus solution

This project contains two application communnicating via Azure Service Bus.

## Creation App
This app allows:
- add user to database (id, e-mail, first name, last name, age, flag active = false
- sending message to service bus about user adding
- edit user
- sending message to service bus about user editing
- list active users

## Validation App
This app has consumers for adding and editing events which fire user validation. After successful validation active flag is set to true.
