import { Injectable } from '@angular/core';
import { Apollo, Query } from 'apollo-angular';
import { map, Observable } from 'rxjs';
import { CapabilityResponse, CapabilitiesResponse, SEARCH_CAPABILITY_QUERY, CAPABILITY_QUERY, Capability, SearchCapabilitiesResponse } from 'src/app/shared';

@Injectable({
    providedIn: 'root',
  })
  export class GetCapabilitiesQL extends Query<CapabilitiesResponse> {
      document = SEARCH_CAPABILITY_QUERY;
  }

  @Injectable({
    providedIn: 'root',
  })
  export class GetCapabilityQL extends Query<CapabilityResponse> {
      document = CAPABILITY_QUERY;
  }

  @Injectable({
    providedIn: 'root'
  })
  export class CapabilityService {
    constructor(
      private apollo: Apollo,
      private getCapabilityQL: GetCapabilityQL,
      private getCapabilitiesQL: GetCapabilitiesQL
      )
      {
          
      }

      save (name: string, description: string) {
          
      }

      getCapability(capabilityId: string): Observable<CapabilityResponse> {
        return this.getCapabilityQL.watch({
          capabilityId: capabilityId
        }).valueChanges.pipe(map(result => result.data));
      }

      searchCapabilities(searchQuery: string, offset: number, limit: number): SearchCapabilitiesResponse {
        const feedQuery = this.apollo.watchQuery<CapabilitiesResponse>({
          query: SEARCH_CAPABILITY_QUERY,
          variables: {
            searchQuery: searchQuery,
            offset: offset,
            limit: limit
          },
          fetchPolicy: 'cache-first',
        });
    
        const fetchMore: (capabilities: Capability[]) => void = (capabilities: Capability[]) => {
          feedQuery.fetchMore({
            variables: {
              offset: capabilities.length,
            }
          });
        }
        return { data: feedQuery.valueChanges.pipe(map(result => result.data)), fetchMore: fetchMore };
    }
}
