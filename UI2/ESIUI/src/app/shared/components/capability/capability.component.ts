import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CapabilityService } from 'src/app/capabilities/capability.service';

@Component({
  selector: 'app-capability',
  templateUrl: './capability.component.html',
  styleUrls: ['./capability.component.css']
})
export class CapabilityComponent implements OnInit, OnDestroy {
  form: FormGroup = new FormGroup({
    name: new FormControl('', [ Validators.required 
      ]),
    description: new FormControl('', [ Validators.required])
  });
  errorMessage: string | null = '';
  private subscription: Subscription | null = null;

  constructor(
  private capabilityService: CapabilityService,
  private snackBar: MatSnackBar,
  private router: Router) { }
  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  ngOnInit(): void {
  }

  submit() {
    const { name, description } = this.form.value;
    if (!this.form.valid) {
      this.errorMessage = 'Please enter valid information';
      return;
    }
    this.subscription = this.capabilityService.save(name, description).subscribe();
  }
}
