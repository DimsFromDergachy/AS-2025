# Check no http
GET http://localhost:5002/
[Options]
skip:true
HTTP 400

GET http://localhost:5002/openapi/v1.json
HTTP 200

# Check login page
GET https://localhost:7198/login
[Options]
skip: true
HTTP 200
[Asserts]
xpath "string(/html/head/title)" contains "Atom Skills 2025"

