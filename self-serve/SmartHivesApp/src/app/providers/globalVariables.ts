import { Injectable } from '@angular/core';

@Injectable()
export class GlobalVariables {
    constructor() {
        // Set the default global variables
        this.appName = "Smart Hives"
    }
    public appName: string; 
}