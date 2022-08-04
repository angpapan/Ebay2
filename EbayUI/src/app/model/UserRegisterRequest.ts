export class UserRegisterRequest {
  "username": string = "";
  "password": string = "";
  "verifyPassword": string = "";
  "firstName": string = "";
  "lastName": string = "";
  "email": string = "";
  "phoneNumber": string = "";
  "street": string = "";
  "streetNumber": number = -1;
  "city": string = "";
  "postalCode": string = "";
  "country": string = "";
  "vatNumber": string = "";

  constructor(username: string, password: string, verifyPassword: string, firstName: string, lastName: string, email: string, phoneNumber: string, street: string, streetNumber: number, city: string, postalCode: string, country: string, vatNumber: string) {
    this.username = username;
    this.password = password;
    this.verifyPassword = verifyPassword;
    this.firstName = firstName;
    this.lastName = lastName;
    this.email = email;
    this.phoneNumber = phoneNumber;
    this.street = street;
    this.streetNumber = streetNumber;
    this.city = city;
    this.postalCode = postalCode;
    this.country = country;
    this.vatNumber = vatNumber;
  }
}
