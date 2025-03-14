POST https://{{hostname}}:5004/api/identity/login
    {"email":"WRONG_USER","password":"{{password}}"}
    HTTP 401

POST https://{{hostname}}:5004/api/identity/login
    {"email":"{{username}}","password":"WRONG_PASSWORD"}
    HTTP 401

POST https://{{hostname}}:5004/api/identity/login
    {"email":"{{username}}","password":"{{password}}"}
    HTTP 200
