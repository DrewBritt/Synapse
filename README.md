# Synapse
A grade book webapp that placed 2nd in the US Congressional App Challenge 2019 for TX31.

[![Build Status](https://travis-ci.com/DrewBritt/Synapse.svg?token=4rByHCezJSmruU5cvesW&branch=master)](https://travis-ci.com/DrewBritt/Synapse)

[Video Demonstration](https://www.youtube.com/watch?v=2yUqcTfEIHo)

# Features
Students:
* Can view their class schedule assigned by Administrators
* Can view grades (individual assignments and class average) set by teachers

Teachers:
* Can manage classes assigned by Administrators
* Can create assignments, and classify them under categories (support for grade weights)
* Can give grades for assignments to students
* Can submit behavior referrals for students

Administrators:
* Can add, delete, and manage students, teachers, and classes.
* Can view student's schedules and grades
* Can view teacher's schedules and class assignment/student grades
* Can manage submitted referrals

# Stack
* ASP.NET Core 2.2 MVC w/ Razor pages

* Authentication and Authorization using .NET Identity

* Database management with MySQL and Entity Framework Core

* CSS with Bulma and SASS

* JS with JQuery

# License
Synapse is licensed under [MIT](https://github.com/drewbritt/synapse/blob/master/LICENSE) by [Drew Britt](https://github.com/drewbritt) & [Lyon Jenkins](https://github.com/lyonjenkins).
