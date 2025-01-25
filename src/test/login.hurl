# Check no http
GET http://localhost:5052/
HTTP 400

# Check login page
GET https://localhost:7198/login
HTTP 200
[Asserts]
xpath "string(/html/head/title)" contains "Atom Skills 2025"

