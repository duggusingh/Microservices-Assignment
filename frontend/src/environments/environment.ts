export const environment = {
  production: false,
  // Ports matching docker-compose.yml mapping (8081 for Product, 8082 for Order)
  productApiUrl: 'http://localhost:8081/api/products',
  orderApiUrl: 'http://localhost:8082/api/orders'
};
