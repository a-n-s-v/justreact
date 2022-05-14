import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';

import { CapabilitiesRoutingModule } from './capabilities-routing.module';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    CapabilitiesRoutingModule,
    SharedModule
  ]
})
export class CapabilitiesModule { }
