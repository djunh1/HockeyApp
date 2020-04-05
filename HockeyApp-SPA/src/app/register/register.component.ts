import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker/public_api';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  // @Input() valuesFromHome: any;  ---> Must match html , for parent in the [] -- In child use the name
  @Output() cancelRegister = new EventEmitter();  // Outputs generate events, so must be new event objects. In parent use ()
  user: User;
  registerForm: FormGroup; // Use for validated forms.
  bsConfig: Partial<BsDatepickerConfig>; // Date picker configuration, a partial class makes 

  constructor(private authService: AuthService, private alertify: AlertifyService, 
              private fb: FormBuilder, private router: Router) {}

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-red'
    };
    this.createRegisterForm();
  }

  createRegisterForm(){
    // Use the form builder
    this.registerForm = this.fb.group({
      playerPosition: ['forward'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      lastActive: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(20)]],
      confirmPassword: ['', Validators.required]
    }, {
      validators: this.passwordMatchValidator
    });
  }

  // This validation is performed on the form group level
  passwordMatchValidator(g: FormGroup){
    return g.get('password').value === g.get('confirmPassword').value ? null : {'mismatch': true};
  }

  register(){
    if (this.registerForm.valid) {
      this.user = Object.assign({}, this.registerForm.value); // Clones all values into an empty object, assigns to user
      this.authService.register(this.user).subscribe( () => {
        this.alertify.success('Registration sucessful, welcome to the Hockey App');
      }, error => {
        this.alertify.error(error);
      }, () => {
        this.authService.login(this.user).subscribe( () => {
          this.router.navigate(['/rinks']);
        });
      });
    }
  }

  cancel(){
    this.cancelRegister.emit(false);
    this.alertify.message('Logged out Successfully.');
  }

}
