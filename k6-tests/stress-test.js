import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
    stages: [
        { duration: '1m', target: 10 }, // Ramp-up to 10 VUs in x minutes
        { duration: '2m', target: 20 }, // Ramp-up to 20 VUs in x minutes
        { duration: '3m', target: 30 }, // Peak load for x minutes
        { duration: '1m', target: 0 }, // Scale down
    ],
};

export default function () {
        // Test POST create product
    let res = http.post('http://api:80/api/Crud', JSON.stringify({
        name: 'Test Product',
        price: 10.0,
        description: 'A product for testing'
    }), { headers: { 'Content-Type': 'application/json' } });
    check(res, { 'POST create product status is 201': (r) => r.status === 201 });
    sleep(1);

    // Test GET product by ID
    let productId = res.json().product.id;
    res = http.get(`http://api:80/api/Crud/${productId}`);
    check(res, { 'GET product by ID status is 200': (r) => r.status === 200 });
    sleep(1);

    // Test PUT update product
    res = http.put(`http://api:80/api/Crud/${productId}`, JSON.stringify({
        name: 'Updated Test Product',
        price: 15.0,
        description: 'An updated product for testing'
    }), { headers: { 'Content-Type': 'application/json' } });
    check(res, { 'PUT update product status is 200': (r) => r.status === 200 });
    sleep(1);

    // Test GET all products
    res = http.get('http://api:80/api/Crud');
    check(res, { 'GET all products status is 200': (r) => r.status === 200 });
    sleep(1);

    // Test DELETE product
    res = http.del(`http://api:80/api/Crud/${productId}`);
    check(res, { 'DELETE product status is 200': (r) => r.status === 200 });
    sleep(1);
}
