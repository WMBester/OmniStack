import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
    vus: 20,  // Moderate load
    duration: '1m',
};

export default function () {
    // Test POST create product
    let res = http.post('http://api:80/api/Crud', JSON.stringify({
        name: 'Test Product',
        price: 10.0,
        description: 'A product for testing'
    }), { headers: { 'Content-Type': 'application/json' } });
    check(res, {
        'POST create product status is 201': (r) => r.status === 201,
        'POST create product response time is < 200ms': (r) => r.timings.duration < 200,
    });
    sleep(1);

    // Test GET product by ID
    let productId = res.json().product.id;
    res = http.get(`http://api:80/api/Crud/${productId}`);
    check(res, {
        'GET product by ID status is 200': (r) => r.status === 200,
        'GET product by ID response time is < 200ms': (r) => r.timings.duration < 200,
    });
    sleep(1);

    // Test PUT update product
    res = http.put(`http://api:80/api/Crud/${productId}`, JSON.stringify({
        name: 'Updated Test Product',
        price: 15.0,
        description: 'An updated product for testing'
    }), { headers: { 'Content-Type': 'application/json' } });
    check(res, {
        'PUT update product status is 200': (r) => r.status === 200,
        'PUT update product response time is < 200ms': (r) => r.timings.duration < 200,
    });
    sleep(1);

    // Test GET all products
    res = http.get('http://api:80/api/Crud');
    check(res, {
        'GET all products status is 200': (r) => r.status === 200,
        'GET all products response time is < 200ms': (r) => r.timings.duration < 200,
        'GET all products response contains products': (r) => r.json().length > 0,
    });
    sleep(1);

    // Test DELETE product
    res = http.del(`http://api:80/api/Crud/${productId}`);
    check(res, {
        'DELETE product status is 200': (r) => r.status === 200,
        'DELETE product response time is < 200ms': (r) => r.timings.duration < 200,
    });
    sleep(1);
}
