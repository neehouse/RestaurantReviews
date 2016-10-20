RestaurantReviews
=================

The Problem
---------------
Truefit is in the midst of building a mobile application that will let restaurant patrons rate the restaurant in which they are eating. As part of the build, we need to develop an API that will accept and store the ratings and other sundry data. 

For this project, we would like you to build this api. Feel free to add your own twists and ideas to what type of data we should collect and return, but at a minimum your API should be able to:

1. Get a list of restaurants by city
2. Post a restaurant that is not in the database
3. Post a review for a restaurant
4. Get of a list of reviews by user
5. Delete a review

The Fine Print
---------------
Please use whatever technology and techniques you feel are applicable to solve the problem. We suggest that you approach this exercise as if this code was part of a larger system. The end result should be representative of your abilities and style.

Please fork this repository. When you have completed your solution, please issue a pull request to notify us that you are ready.

Have fun.

===============
Developer Notes
===============

General
1. For larger projects, I would use IoC containers to do injection.  I wired the API Controllers to use the default constructor in a similar structure, but with direct references.
2. Unit Tests - Coming soon.
3. Using EntityFramework.  Did Model (designer first) in this scenario.

CMMI.Web
---------------
* Error Handling: 
** By Default, I have wired each method to do ModelState.IsValid in the ModelStateValidationAttribute, which is wired up in the WebApiConfig.
** Rather than implementing try/catch blocks for each exception type in the API methods, The WebApi config allows for the replacement of the Exception Handler, as such, it looks at the exception type, and determines the correct statusCode.  See the Exceptions folder in CMMI.Business
*** NotFoundException
*** InvalidPermissionsException
*** Etc.

CMMI.Business
---------------
* Business has view models that are delivered to the api.  The Business layer works with the entities, and they and the context are not exposed to the API. 
