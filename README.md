# project-IS

# Database

- Create a SQL Database in `/Project/middleware-d26/App_Data` named `DBmiddleware.mdf`.
- Initialize the database.
```bash
Install-Package EntityFramework
Update-Database
```

# API Documentation

## Applications

### Create Application - `createNewApplication`

```http
POST http://localhost:65252/api/somiod
Content-Type: application/xml
```

```xml
<EntityRequest xmlns="Middleware-d26">
    <res_type>application</res_type>
    <name>NewApplication3</name>
</EntityRequest>
```

### Get Application - `getApplication`

```http
GET http://localhost:65252/api/somiod/NewApplication
Accept: application/xml
```

### Modify Application - `modifyApplication`

```http
PUT http://localhost:65252/api/somiod/NewApplication
Content-Type: application/xml
```

```xml
<EntityRequest xmlns="Middleware-d26">
    <res_type>application</res_type>
    <name>ModifiedApplication</name>
</EntityRequest>
```

### Delete Application - `deleteApplication`

```http
DELETE http://localhost:65252/api/somiod/NewApplication
```

## Containers

### Create Container - `createNewContainer`

```http
POST http://localhost:65252/api/somiod/App1
Content-Type: application/xml
```

```xml
<EntityRequest xmlns="Middleware-d26">
    <res_type>container</res_type>
    <name>NewContainer</name>
</EntityRequest>
```

### Get Container - `getContainer`

```http
GET http://localhost:65252/api/somiod/App1/NewContainer
Accept: application/xml
```

### Modify Container - `modifyContainer`

```http
PUT http://localhost:65252/api/somiod/App1/NewContainer
Content-Type: application/xml
```

```xml
<EntityRequest xmlns="Middleware-d26">
    <res_type>container</res_type>
    <name>ModifiedContainer</name>
</EntityRequest>
```

### Delete Container - `deleteContainer`

```http
DELETE http://localhost:65252/api/somiod/App1/ModifiedContainer
```

## Subscriptions

### Create Subscription - `createSubscription`

```http
POST http://localhost:65252/api/somiod/App1/Container1
Content-Type: application/xml
```

```xml
<EntityRequest xmlns="Middleware-d26">
    <res_type>subscription</res_type>
    <subscription>
        <name>NewSubscription</name>
        <event>YourEvent</event>
        <endpoint>YourEndpoint</endpoint>
    </subscription>
</EntityRequest>
```

### Get Subscription - `getSubscription`

```http
GET http://localhost:65252/api/somiod/App1/Container1/sub/NewSubscription
Accept: application/xml
```

### Delete Subscription - `deleteSubscription`

```http
DELETE http://localhost:65252/api/somiod/App1/Container1/sub/NewSubscription
```

## Datas

### Create Data - `createData`

```http
POST http://localhost:65252/api/somiod/App1/Container1
Content-Type: application/xml
```

```xml
<EntityRequest xmlns="Middleware-d26">
    <res_type>data</res_type>
    <data>
        <name>NewData</name>
        <content>YourContent</content>
    </data>
</EntityRequest>
```

### Get Data - `getData`

```http
GET http://localhost:65252/api/somiod/App1/Container1/data/NewData
Accept: application/xml
```

### Delete Data - `deleteData`

```http
DELETE http://localhost:65252/api/somiod/App1/Container1/data/NewData
```

## Discovery

### Discover Applications - `discoverApplications`

```http
GET http://localhost:65252/api/somiod
Accept: application/xml
somiod-discover: application
```

### Discover Containers - `discoverContainers`

```http
GET http://localhost:65252/api/somiod/App1
Accept: application/xml
somiod-discover: container
```

### Discover Subscriptions - `discoverSubscriptions`

```http
GET http://localhost:65252/api/somiod/App1/Container1
Accept: application/xml
somiod-discover: subscription
```

### Discover Datas - `discoverDatas`

```http
GET http://localhost:65252/api/somiod/App1/Container1
Accept: application/xml
somiod-discover: data
```

---
