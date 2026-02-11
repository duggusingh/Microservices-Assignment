import { Component } from '@angular/core';
import { OrderService } from '../../services/order.service';

@Component({
  selector: 'app-order-create',
  templateUrl: './order-create.component.html'
})
export class OrderCreateComponent {
  productId: number = 0;
  qty: number = 1;
  message: string = '';

  constructor(private orderService: OrderService) {}

  placeOrder() {
    const order = {
      customerName: 'Durgesh Customer', // Hardcoded or from input
      orderItems: [{ productId: this.productId, qty: this.qty }]
    };

    this.orderService.placeOrder(order).subscribe({
      next: () => this.message = 'Order Placed Successfully!',
      error: (err) => this.message = 'Failed: ' + err.error.message
    });
  }
}
