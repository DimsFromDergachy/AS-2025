GET https://{{hostname}}:5002/
[Options]
insecure: true # TODO: Self signed certificate
HTTP 404

GET https://{{hostname}}:5002/api/trait/list
[Options]
insecure: true
HTTP 200
