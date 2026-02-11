export interface OrderItem {
    productId: number;
    qty: number;
}

export interface Order {
    orderId?: number;
    customerName: string;
    orderDate?: Date;
    orderItems: OrderItem[];
}
