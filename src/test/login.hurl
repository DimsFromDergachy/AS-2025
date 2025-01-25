# Check no http
GET http://localhost:5004/
HTTP 400

# Check login page
GET https://localhost:5004/login
HTTP 200
[Asserts]
xpath "string(/html/head/title)" contains "Atom Skills 2025"

