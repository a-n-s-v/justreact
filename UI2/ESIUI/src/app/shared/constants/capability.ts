import { gql } from 'apollo-angular';
import { CAPABILITY_FIELDS_FRAGMENT, BASIC_CAPABILITY_FIELDS_FRAGMENT } from './capability.fragment';

export const CAPABILITY_QUERY = gql`
 ${BASIC_CAPABILITY_FIELDS_FRAGMENT}
 query getCapability($capabilityId: ID!){  
    getCapability(capabilityId:$capabilityId){ __typename ...CapabilityFields }
 } 
`;
export const SEARCH_CAPABILITY_QUERY = gql` 
 ${CAPABILITY_FIELDS_FRAGMENT}
 query searchCapabilities($searchQuery: String!){  
  searchCapabilities(searchQuery:$searchQuery){ __typename ...BasicCapabilityFields }
 }  
`;