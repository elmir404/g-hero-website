# API Endpoints Reference

## Base URL
- Development: `http://localhost:50001/api`
- Production: `https://your-api-domain.com/api`

## Authentication

All admin endpoints require JWT Bearer token in the Authorization header:
```
Authorization: Bearer {your-jwt-token}
```

---

## Public Endpoints

### GET /api/home
Get complete home page data for all sections.

**Query Parameters:**
- `lang` (optional): Language code (`en`, `az`, `jp`). Default: `en`

**Response:**
```json
{
  "hero": {
    "id": 1,
    "languageCode": "en",
    "title": "G-Hero Company",
    "subtitle": "Japanese Quality to the World...",
    "description": "...",
    "primaryButtonText": "Contact Us",
    "primaryButtonUrl": "#contact",
    "secondaryButtonText": "View Services",
    "secondaryButtonUrl": "#domains"
  },
  "philosophy": {
    "id": 2,
    "sectionKey": "philosophy.title",
    "languageCode": "en",
    "title": "Philosophy",
    "content": "...",
    "subtitle": null,
    "additionalData": null
  },
  "mission": { ... },
  "vision": { ... },
  "values": { ... },
  "domains": { ... },
  "about": { ... },
  "services": [ ... ],
  "testimonials": [ ... ],
  "faqs": [ ... ],
  "contactInfo": { ... }
}
```

### GET /api/home/hero
Get hero section only.

**Query Parameters:**
- `lang` (optional): Language code. Default: `en`

### GET /api/home/services
Get services list.

**Query Parameters:**
- `lang` (optional): Language code. Default: `en`

**Response:**
```json
[
  {
    "id": 1,
    "languageCode": "en",
    "title": "Service Name",
    "description": "Service description",
    "iconName": "icon-name",
    "iconUrl": null,
    "imageUrl": null,
    "displayOrder": 1
  }
]
```

### GET /api/home/testimonials
Get testimonials list.

### GET /api/home/faqs
Get FAQs list.

### GET /api/home/contact-info
Get contact information.

---

### POST /api/contact
Submit contact form.

**Request Body:**
```json
{
  "name": "John Doe",
  "email": "john@example.com",
  "subject": "Inquiry",
  "company": "Company Name",
  "message": "Message content here"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Thank you for your message! We will get back to you soon.",
  "id": 123
}
```

**Error Response:**
```json
{
  "errors": {
    "Email": ["Invalid email address"],
    "Message": ["Message is required"]
  }
}
```

---

### POST /api/auth/login
Login and get JWT token.

**Request Body:**
```json
{
  "username": "admin",
  "password": "Admin123!"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin",
  "email": "admin@g-hero-company.com",
  "fullName": "Administrator",
  "roles": ["Admin"],
  "expiresAt": "2024-12-09T10:00:00Z"
}
```

**Error Response:**
```json
{
  "message": "Invalid username or password"
}
```

### GET /api/auth/me
Get current authenticated user info.

**Headers:**
- `Authorization: Bearer {token}`

**Response:**
```json
{
  "username": "admin",
  "email": "admin@g-hero-company.com",
  "roles": ["Admin"]
}
```

---

## Admin Endpoints

### Contact Messages

#### GET /api/admin/contact
List all contact messages (paginated).

**Query Parameters:**
- `page` (optional): Page number. Default: 1
- `pageSize` (optional): Items per page. Default: 20

**Headers:**
- `Authorization: Bearer {token}`

**Response:**
```json
{
  "data": [
    {
      "id": 1,
      "name": "John Doe",
      "email": "john@example.com",
      "subject": "Inquiry",
      "company": "Company",
      "message": "Message",
      "createdAt": "2024-12-02T10:00:00Z",
      "isRead": false,
      "readAt": null
    }
  ],
  "total": 50,
  "page": 1,
  "pageSize": 20,
  "totalPages": 3
}
```

#### GET /api/admin/contact/unread
Get unread messages only.

**Response:**
```json
{
  "data": [ ... ],
  "count": 5
}
```

#### GET /api/admin/contact/{id}
Get single message by ID.

#### PATCH /api/admin/contact/{id}/read
Mark message as read.

**Response:**
```json
{
  "message": "Message marked as read"
}
```

#### PATCH /api/admin/contact/{id}/unread
Mark message as unread.

#### DELETE /api/admin/contact/{id}
Delete message.

---

### Content Management

#### GET /api/admin/content
List all content sections.

**Query Parameters:**
- `lang` (optional): Filter by language code

**Response:**
```json
{
  "data": [
    {
      "id": 1,
      "sectionKey": "hero.title",
      "languageCode": "en",
      "title": "G-Hero Company",
      "content": null,
      "subtitle": null,
      "additionalData": null,
      "displayOrder": 1,
      "isActive": true
    }
  ]
}
```

#### GET /api/admin/content/{id}
Get section by ID.

#### POST /api/admin/content
Create new content section.

**Request Body:**
```json
{
  "sectionKey": "hero.title",
  "languageCode": "en",
  "title": "G-Hero Company",
  "content": null,
  "subtitle": null,
  "additionalData": null,
  "displayOrder": 1,
  "isActive": true
}
```

**Response:**
```json
{
  "id": 123,
  "message": "Content section created successfully"
}
```

#### PUT /api/admin/content/{id}
Update existing section.

**Request Body:** Same as POST

#### DELETE /api/admin/content/{id}
Delete section.

#### PATCH /api/admin/content/{id}/toggle-active
Toggle active status.

---

## Error Responses

All endpoints return standard error format:

**400 Bad Request:**
```json
{
  "errors": {
    "Email": ["Invalid email address"]
  }
}
```

**401 Unauthorized:**
```json
{
  "message": "Unauthorized"
}
```

**404 Not Found:**
```json
{
  "message": "Resource not found"
}
```

**500 Internal Server Error:**
```json
{
  "message": "An error occurred"
}
```

