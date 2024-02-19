import { useParamStore } from '@/hooks/useParamsStore'
import React from 'react'
import Heading from './Heading';
import { Button, ButtonGroup } from 'flowbite-react';

type Props = {
    title?: string,
    subtitle?: string,
    showReset?: boolean
}

export default function EmptyFilter({
    title = 'No matches for this filter',
    subtitle = 'Try changing ot resetting the filter',
    showReset 
}: Props) {
    const reset = useParamStore(state => state.reset);

  return (
    <div className='h-[40vh] flex flex-col gap-2 justify-center items-center shadow-lg'>
        <Heading title={title} subtitel={subtitle} center/>
        <div className='mg-4'>
            {showReset && (
                <Button outline onClick={reset}>Remove Filters</Button>
            )}
        </div>
    </div>
  )
}
