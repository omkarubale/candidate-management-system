export class SignInDto {
  email: string;
  password: string;
}

export class Candidate {
  Email: string;
  Password: string;
  ConfirmPassword: string;
  FirstName: string;
  LastName: string;
  FullName: string;
  ShortName: string;
  UserType: number;
  DateOfBirth: Date;
}
