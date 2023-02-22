Feature: LibraryAPI

In order to easily manage my books
As a user of LibraryAPI
I want to be able to do it via API


Scenario: Get all books using valid input
	Given that I have some books:
	| Id | Title             | Description         | Author         |
	| 1  | The Hobbit        | Book about a hobbit | J.R.R. Tolkien |
	| 2  | The Da Vinci Code | Murder mistery      | Dan Brown      |
	When I send a GET request for all books
	Then I should get all books with status code 200


Scenario: Get a book using valid input
	Given that I have a book
	When I send a GET request with the given book id
	Then I should recieve that book with the status code 200


Scenario: Update a book using valid input
	Given that I have a book
	When I send a PUT request with new data for the given book
	Then that given book should be updated with a status code 204


Scenario: Delete a book using valid input
	Given that I have a book
	When I send a DELETE request with the given book id
	Then the book should be deleted with the status code 200