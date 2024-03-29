'use client'

import { error } from 'console';
import React, { useEffect, useState } from 'react'
import AuctionCard from './AuctionCard';
import { Auction, PagedResult } from '@/types';
import AppPagitation from '../components/AppPagitation';
import { getData } from '../actions/auctionActions'
import Filters from './Filters';
import { useParamStore } from '@/hooks/useParamsStore';
import { shallow } from 'zustand/shallow';
import qs from 'query-string';
import EmptyFilter from '../components/EmptyFilter';

export default function Listings() {
  const [data,setData] = useState<PagedResult<Auction>>();
  const params = useParamStore(state => ({
    pageNumber: state.pageNumber,
    pageSize: state.pageSize,
    searchTerm: state.searchTerm,
    orderBy: state.orderBy,
    filterBy: state.filterBy 
  }), shallow)

  const setParams = useParamStore(state => state.setParams);
  const url= qs.stringifyUrl({url:'', query: params})

  function setPageNumber(pageNumber: number){
    setParams({pageNumber})
  }

useEffect(() => {
getData(url).then(data => {
setData(data)
})
},[url])

if(!data) return <h3>Loading...</h3>

if(data.totalCount === 0) return <EmptyFilter showReset/>
  return (
    <>
    <Filters />
        <div className='grid grid-cols-4 gap-6'>
        {data.results.map(auction=> (
            <AuctionCard auction={auction} key={auction.id} />
        ))}
    </div>
    <div className='flex justify-center mt-4'>
      <AppPagitation pageChanged={setPageNumber} currentPage={params.pageNumber} pageCount={data.pageCount} />
     </div>
    </>

  )
}
