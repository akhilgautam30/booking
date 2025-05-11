@UpdateBooking
Feature: Update Booking API
    As an API consumer
    I want to update existing bookings
    So I can modify reservation details

@UpdateBooking @HappyPath
Scenario: Successfully update booking with valid data using token authentication
    Given I have a valid authentication token
    When I update booking ID 1 with valid data using token authentication:
        | firstname | lastname | totalprice | depositpaid | checkin     | checkout    | additionalneeds |
        | James     | Brown    | 111        | true        | 2018-01-01  | 2019-01-01  | Breakfast       |
    Then the response status code should be 200
    And the response should match the updated booking data

@UpdateBooking @HappyPath
Scenario: Successfully update booking with valid data using Basic Auth
    When I update booking ID 1 with valid data using Basic Auth:
        | firstname | lastname | totalprice | depositpaid | checkin     | checkout    | additionalneeds |
        | Sarah     | Smith    | 222        | false       | 2023-01-01  | 2023-01-05  | WiFi            |
    Then the response status code should be 200
    And the response should match the updated booking data

@UpdateBooking @Authentication
Scenario: Attempt to update booking without authentication
    When I update booking ID 1 with valid data without authentication:
        | firstname | lastname | totalprice | depositpaid | checkin     | checkout    | additionalneeds |
        | James     | Brown    | 111        | true        | 2018-01-01  | 2019-01-01  | Breakfast       |
    Then the response status code should be 403

@UpdateBooking @Authentication
Scenario: Attempt to update booking with invalid token
    Given I have an invalid authentication token
    When I update booking ID 1 with valid data using token authentication:
        | firstname | lastname | totalprice | depositpaid | checkin     | checkout    | additionalneeds |
        | James     | Brown    | 111        | true        | 2018-01-01  | 2019-01-01  | Breakfast       |
    Then the response status code should be 403

@UpdateBooking @Validation
Scenario Outline: Attempt to update booking with invalid data
    Given I have a valid authentication token
    When I update booking ID 1 with the following invalid data:
        | firstname | lastname | totalprice | depositpaid | checkin     | checkout    | additionalneeds |
        | <first>   | <last>   | <price>    | <deposit>   | <checkin>   | <checkout>  | <needs>         |
    Then the response status code should be 400

    Examples:
        | first | last  | price | deposit   | checkin      | checkout     | needs      |
        |       | Brown | 111   | true      | 2018-01-01   | 2019-01-01   | Breakfast  |
        | James |       | 111   | true      | 2018-01-01   | 2019-01-01   | Breakfast  |
        | James | Brown | -1    | true      | 2018-01-01   | 2019-01-01   | Breakfast  |
        | James | Brown | 111   | not_bool  | 2018-01-01   | 2019-01-01   | Breakfast  |
        | James | Brown | 111   | true      | invalid-date | 2019-01-01   | Breakfast  |
        | James | Brown | 111   | true      | 2018-01-01   | invalid-date | Breakfast  |
        | James | Brown | 111   | true      | 2018-01-01   | 2019-01-01   |            |