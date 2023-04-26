# Tic Tac Toe API

    This Tic-Tac-Toe API is a .NET 6.0 Web API that allows users to manage games of Tic-Tac-Toe through HTTP requests.
    The API includes endpoints that accept JSON strings as input and return JSON objects as output.
    The API is designed to run on Docker Compose, 
    Documentation for how to run the API and how to use the endpoints is provided below.
    As well as layed out in the frontend UI provided by swagger
    
    
#### question: 
What is the appropriate OAuth 2/OIDC grant to use for a web application using a SPA (Single Page Application) and why?
    
    There is two grant options for SPA's: 
    
    - the Authorization Code Flow with Proof Key for Code Exchange (PKCE)
        - This is one recommended because:
            : Access Token is not exposed on the client side
            : can return Refresh Tokens
        
    - the Implicit Flow with Form Post.

## Install

    Download docker-compose file from release

## Run the app

    docker-compose up --build







## Initilize TTT game

### Curl

    curl -X 'GET' \ 'http://localhost/initgame' \ -H 'accept: */*'

### Request URL

`GET /initgame`

### Response body

    {
        "gameID": "6448a5861060bc2ddace3f22",
        "player1_ID": "b1b815bc-dcfc-4a94-9a0c-39e7b5b53190",
        "player2_ID": "244f03da-faba-4415-910f-586f8520911f"
    }







## Get list of actively playin games

### Curl
    curl -X 'GET' \ 'http://localhost/listActiveGames' \ -H 'accept: */*'

### Request URL
`GET /listActiveGames`

### Response body
    [
      {
        "gameid": "6448a5861060bc2ddace3f22",
        "player_1": "b1b815bc-dcfc-4a94-9a0c-39e7b5b53190",
        "player_2": "244f03da-faba-4415-910f-586f8520911f",
        "moves": 0
      },
      {
        "gameid": "example",
        "player_1": "exmaple",
        "player_2": "exmaple",
        "moves": 7
      }
    ]
    
    
    
    
    
    
    
    
    
## Register a player move

### Curl
    curl -X 'PUT' \ 'http://localhost/registermove/6448a5861060bc2ddace3f22' \
    -H 'accept: */*' \
    -H 'Content-Type: application/json' \
    -d '{
        "player": "b1b815bc-dcfc-4a94-9a0c-39e7b5b53190",
        "col": 1,
        "row": 1
    }'

### Request URL

`GET /registerMove/{id}`

### Request Body
    {
      "player": "b1b815bc-dcfc-4a94-9a0c-39e7b5b53190",
      "col": 1,
      "row": 1
    }


### Response Body
    
#### Normal Move response

    {
      "success": true
    }

#### Winning Move response

    {
      "success": true,
      "status" = "win",
      "winner" = "b1b815bc-dcfc-4a94-9a0c-39e7b5b53190"
    }
    
#### Tie Game response

    {
      "success": true,
      "status" = "tie"
    }
