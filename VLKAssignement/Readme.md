# Van Lanschot Kempen Assignement

## Decisions made
- SQL Server as database (it uses localdb).
- Entity Framework
- Swagger for API documentation. The application starts with the swagger page (swagger/v1/swagger.json)
- Unit testing using xUnit
- I used the public api https://api.exchangeratesapi.io/latest to retrieve exchange rates and once the rates are retrieved they are stored locally in the db so we don't have to access to this external api all the time.
- Once the application starts for first time it creates the db and also seeds it with a user and an account.

## End points
- To get the users from the db you need to do a GET to the end point: api/user so you can collect the user id needed to proceed.
- To create a transfer you need to do a POST to the endpoint: api/transfer with the following body:
```json
{
	"userId":,
	"amount": ,
	"destinationAccountNumber":,
	"destinationCurrencyCode":
}
```
- To sign a transfer you will need the transfer id that you got from the previous step, and do a POST to api/transfer/sign with the transfer id in the body:
- To retrieve the cart you need to do a GET to the following end point: api/cart/user/{userId} specifying the user id in the url.
- To retrieve the transactions you need to do a GET to the following end point: api/transaction/user/{userId} specifying the user id in the url.