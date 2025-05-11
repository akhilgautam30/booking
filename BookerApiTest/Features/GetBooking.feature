@GetBooking
Feature: Booking Details API
    As an API consumer
    I want to retrieve booking details
    So I can view complete booking information

@GetBooking @HappyPath
Scenario Outline: Get booking details with valid ID - Happy Path
    Given I request booking details for ID <bookingId> with Accept: application/json header
    When I request booking details for that ID
    Then the response status code should be 200
    And the response should contain valid booking details with JSON content type
    And the booking details should have all required fields with correct types

    Examples:
    | bookingId |
    | 1         |
    | 2         |

@GetBooking @ErrorCases
Scenario Outline: Get booking details with invalid ID - Error Cases
    Given I have an invalid booking ID <bookingId>
    When I request booking details for that ID
    Then the booking details response status code should be <statusCode>

    Examples:
    | bookingId      | statusCode |
    | 999999999      | 404        |
    | invalid_id     | 404        |
    | 0              | 404        |
    | -1             | 404        |