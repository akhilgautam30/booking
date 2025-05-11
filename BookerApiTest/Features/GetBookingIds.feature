@GetBookingIds
Feature: Booking IDs API
    As an API consumer
    I want to retrieve booking IDs with various filters
    So I can find specific bookings efficiently

@GetBookingIds @HappyPath
Scenario Outline: Get booking IDs with valid parameters - Happy Path
    When I request booking IDs with parameters
        | Parameter | Value       |
        | firstname | <firstname> |
        | lastname  | <lastname>  |
        | checkin   | <checkin>   |
        | checkout  | <checkout>  |
    Then the response status code should be 200
    #And the response should contain booking IDs matching the criteria

    Examples:
    | testCase               | firstname | lastname | checkin     | checkout   | expectedStatus    |
    | Name filter only       | Sally     | Brown    |             |            |    200 |
    | Date filter only       |           |          | 2014-03-13  | 2014-05-21 |    200 |
    | Combined name+date     | Sally     | Brown    | 2014-03-13  | 2014-05-21 |    200 |
    | without parms     |           |          |             |            |    200 |

@GetBookingIds @EdgeCases
Scenario Outline: Get booking IDs with edge case parameters
    When I request booking IDs with parameters
        | Parameter | Value       |
        | checkin   | <checkin>   |
        | checkout  | <checkout>  |
    Then the response status code should be <expectedStatus>
    #And the response should <containOrNot> booking IDs

    Examples:
    | testCase                     | checkin     | checkout    | expectedStatus | containOrNot  |
    | Invalid date format          | 2023/02/28  | 2023/03/01 | 200            | not contain    |
    | Checkout before checkin      | 2023-03-01  | 2023-02-28 | 200            | not contain    |
    | future dates                 | 2030-01-01  | 2030-01-10 | 200            | contain        |
    | Far past dates               | 2000-01-01  | 2000-01-10 | 200            | contain        |
    | Single day stay              | 2023-02-28  | 2023-02-28 | 200            | contain        |
    | Maximum date range           | 2010-01-01  | 2030-12-31 | 200            | contain        |

@GetBookingIds @SpecialCharacters
Scenario Outline: Get booking IDs with special characters
    When I request booking IDs with parameters
        | Parameter | Value       |
        | firstname | <firstname> |
        | lastname  | <lastname>  |
    Then the response status code should be 200

    Examples:
    | testCase               | firstname      | lastname       |
    | Hyphenated name        | Anne-Marie     | O'Connor       |
    | Accented characters    | José           | Muñoz          |
    | Space in name          | Mary Jane      | Parker Smith   |
    | Numbers in name        | Agent          | 007            |
    | Special chars          | Johnny#        | $mith          |