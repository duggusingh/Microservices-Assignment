import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrderService } from '../../services/order.service';
import { Order } from '../../models/order.model';

@Component({
  selector: 'app-order-create',
  templateUrl: './order-create.component.html'
})
export class OrderCreateComponent implements OnInit {
  order: Order = { customerName: '', orderItems: [{ productId: 0, qty: 1 }] };
  message: string = '';

  constructor(private route: ActivatedRoute, private orderService: OrderService) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.order.orderItems[0].productId = +params['id'];
    });
  }

  submitOrder() {
    this.orderService.placeOrder(this.order).subscribe({
      next: () => this.message = "Order placed successfully!",
      error: (err) => this.message = err.error.message || "Failed to place order."
    });
  }
}
