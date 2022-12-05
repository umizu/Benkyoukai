# Benkyoukai API Docs

## Create Session

### Create Session Request

```js
POST /sessions
```

```json
{
    "name": "jlpt n4",
    "description": "grammar & vocab",
    "startDateTime": "2022-08-07T20:00:00",
    "endDateTime": "2022-08-07T21:00:00"
}
```

### Create Session Response

```js
201 Created
```

```js
Location: {{host}}/Sessions/{{id}}
```

```json
{
    "id": "00000000-0000-0000-0000-000000000000",
    "name": "jlpt n4",
    "description": "grammar & vocab",
    "startDateTime": "2022-08-07T20:00:00",
    "endDateTime": "2022-08-07T21:00:00",
    "lastModifiedDateTime": "2022-08-05T12:00:00"
}
```

## Get Session

### Get Session Request

```js
GET /Sessions/{{id}}
```

### Get Session Response

```js
200 Ok
```

```json
{
    "id": "00000000-0000-0000-0000-000000000000",
    "name": "jlpt n4",
    "description": "grammar & vocab",
    "startDateTime": "2022-08-07T20:00:00",
    "endDateTime": "2022-08-07T21:00:00",
    "lastModifiedDateTime": "2022-08-05T12:00:00"
}
```
