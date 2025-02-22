import http from 'k6/http';
import { check, sleep } from 'k6';

// Get API URL from environment variable (fallback to 'http://api:80' if not set)
const apiUrl = __ENV.K6_API_URL || 'http://api:80';

export default function () {
    // Generate unique product name using timestamp
    const productName = 'Test Product ' + new Date().toISOString();

    // Test POST create product
    let res = http.post(`${apiUrl}/api/Crud`, JSON.stringify({
        name: productName,
        price: 10.0,
        description: 'A product for testing'
    }), { headers: { 'Content-Type': 'application/json' } });
    check(res, {
        'POST create product status is 201': (r) => r.status === 201,
        'POST create product content-type is application/json': (r) => r.headers['Content-Type'] === 'application/json',
    });
    sleep(1);

    // Extract product ID from the response
    let productId = res.json().product.id;

    // Test GET product by ID
    res = http.get(`${apiUrl}/api/Crud/${productId}`);
    check(res, {
        'GET product by ID status is 200': (r) => r.status === 200,
        'GET product by ID content-type is application/json': (r) => r.headers['Content-Type'] === 'application/json',
    });
    sleep(1);

    // Test PUT update product
    res = http.put(`${apiUrl}/api/Crud/${productId}`, JSON.stringify({
        name: 'Updated ' + productName,
        price: 15.0,
        description: 'An updated product for testing'
    }), { headers: { 'Content-Type': 'application/json' } });
    check(res, {
        'PUT update product status is 200': (r) => r.status === 200,
        'PUT update product content-type is application/json': (r) => r.headers['Content-Type'] === 'application/json',
    });
    sleep(1);

    // Test GET all products
    res = http.get(`${apiUrl}/api/Crud`);
    check(res, {
        'GET all products status is 200': (r) => r.status === 200,
        'GET all products content-type is application/json': (r) => r.headers['Content-Type'] === 'application/json',
    });
    sleep(1);

    // Test DELETE product
    res = http.del(`${apiUrl}/api/Crud/${productId}`);
    check(res, {
        'DELETE product status is 200': (r) => r.status === 200,
        'DELETE product content-type is application/json': (r) => r.headers['Content-Type'] === 'application/json',
    });
    sleep(1);
}
