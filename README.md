# HenryMedsReservationAPI

Architecture/ Project structure Changes
---------------------------------------
- I would move all the business logic that is currently living in the controllers to Services/ Managers
- I would implement a HttpResponseMessage middleware to wrap my outgoing API messages with a HTTP status success, status code and user friendly message as part of the json response body
- Add DTOs so I'm not exposing whole entities to the enpoints
- Add Fluent Validation on DTOs to validate incoming properties of requests
