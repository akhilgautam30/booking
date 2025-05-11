@CreateBooking
Feature: Create Booking
    As a user
    I want to create a new booking
    So that I can reserve a room

    #Background:
    #    Given I set the API endpoint to 'booking'

    @GetBooking @HappyPath
    Scenario: Create a booking with valid data
        When I send a POST request with the following valid booking data:
            | firstname    | lastname | totalprice | depositpaid | checkin     | checkout    | additionalneeds |
            | Jim          | Brown    | 111        | true        | 2018-01-01  | 2019-01-01  | Breakfast       |
        Then the response status code should be 200
        And the response should contain a bookingid
        And the response should match the request data

    @Validation
    Scenario Outline: Create a booking with missing required field
        When I send a POST request with booking data missing the <field> field
        Then the response status code should be 400  
        #values can be 400 or 500 based on the API behavior

        Examples:
            | field          |
            | firstname     |
            | lastname       |
            | totalprice     |
            | depositpaid   |
            | bookingdates  |

    @Validation
    Scenario Outline: Create a booking with invalid field values
        When I send a POST request with booking data where <field> is set to <value>
        Then the response status code should be 400

        Examples:
            | field          | value         |
            | totalprice    | -1            |
            | totalprice    | "invalid"     |
            | depositpaid    | "notboolean"  |
            | bookingdates   | null          |
            | bookingdates   | {}            |
            | bookingdates   | {"checkin":"invalid-date","checkout":"2019-01-01"} |
            | bookingdates   | {"checkin":"2018-01-01","checkout":"invalid-date"} |

    @EdgeCases
    Scenario: Create a booking with empty additional needs
        When I send a POST request with the following valid booking data:
            | firstname | lastname | totalprice | depositpaid | checkin     | checkout    | additionalneeds |
            | Jim       | Brown    | 111        | true        | 2018-01-01  | 2019-01-01  |                 |
        Then the response status code should be 200
        And the response should contain a bookingid
        And the additionalneeds field should be empty

    @EdgeCases
    Scenario: Create a booking with minimum valid data
        When I send a POST request with the following valid booking data:
            | firstname | lastname | totalprice | depositpaid | checkin     | checkout    |
            | J         | B        | 0          | false       | 2018-01-01  | 2018-01-02  |
        Then the response status code should be 200
        And the response should contain a bookingid

    @EdgeCases
    Scenario: Create a booking with maximum field lengths
        When I send a POST request with the following valid booking data:
            | firstname                                                                 | lastname                                                                 | totalprice | depositpaid | checkin     | checkout    | additionalneeds                                                                 |
            | ThisIsAVeryLongFirstNameThatExceedsNormalLimitsButShouldStillWorkForTheApi | ThisIsAVeryLongLastNameThatExceedsNormalLimitsButShouldStillWorkForTheApi | 999999999  | true        | 2018-01-01  | 2019-01-01  | ThisIsAVeryLongAdditionalNeedsFieldThatExceedsNormalLimitsButShouldStillWorkForTheApi |
        Then the response status code should be 200
        And the response should contain a bookingid
        And the response should match the request data