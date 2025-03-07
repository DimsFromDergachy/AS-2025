GET https://localhost:5002/
[Options]
insecure: true
HTTP 404

POST https://localhost:5002/trait
[Options]
insecure: true
{
"code": "12",
"name": "DT",
"description": "DT desc"
}
HTTP 200
