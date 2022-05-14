import { Observable } from "rxjs";
import { Capability } from "../models/capability.model";

export type CapabilityResponse = {
    getCapability: Capability;
}

export type CapabilitiesResponse = {
    searchCapabilities: Capability[];
}

export type SearchCapabilitiesResponse = {
    data: Observable<CapabilitiesResponse>;
    fetchMore: (capabilities: Capability[]) => void
}