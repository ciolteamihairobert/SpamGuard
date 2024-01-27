export class User {
    id = 0;
    email = '';
    passwordHash: ArrayBuffer = new ArrayBuffer(0);
    passwordSalt: ArrayBuffer = new ArrayBuffer(0);
    firstName = '';
    lastName = '';
    role = '';
}