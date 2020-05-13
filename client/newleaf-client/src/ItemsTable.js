import React, { useState, useEffect } from 'react'
import { Table, Input, Button, Grid, Icon, Segment } from 'semantic-ui-react'
import _ from 'lodash';

const ItemsTable = props => {
    const [column, setColumn] = useState(null);
    const [data, setData] = useState(null);
    const [filteredData, setFilteredData] = useState(null);
    const [direction, setDirection] = useState(null);
    const [price, setPrice] = useState('');
    const [searchValue, setSearchValue] = useState('');
    const [confirmingDeletion, setConfirmingDeletion] = useState(false);

    const fetchData = () => {
        fetch('https://acnlapi.azurewebsites.net/api/itemprices/get')
        .then(response => response.json())
        .then((data) => {
            setData(data);
            setFilteredData(data);
        }
        );
    }
    useEffect(() => {
        fetchData();
    }, []);
    const handleSort = (clickedColumn) => {
        if (column !== clickedColumn) {
            setColumn(clickedColumn);
            setFilteredData(_.sortBy(filteredData, [clickedColumn]));
            setDirection('ascending');
            return;
        };
        setFilteredData(filteredData.reverse());
        setDirection(direction === 'ascending' ? 'descending' : 'ascending');
    };

    const handleSearchChange = (e, { value }) => {
        setSearchValue(value);
        if (!value) setFilteredData(data);
        let numberOfMatches = 0;
        let matches = [];
        data.forEach(item => {
            if (item.name.indexOf(value.toLowerCase()) > - 1) {
                numberOfMatches += 1;
                matches.push(item);
                if (numberOfMatches >= 10) {
                    setFilteredData(matches);
                    return;
                }
            }
        })
        setFilteredData(matches);
    }
    const handlePriceChange = (e, { value }) => {
        if (!value) {
            setPrice('');
            return;
        }
        var isnum = /^\d+$/.test(value);
        if (!isnum) {
            return;
        }
        setPrice(value);
    }

    const handleDeleteConfirmation = (shouldDelete, itemName) => {
        setConfirmingDeletion(false);
        if(shouldDelete){
            fetch(`https://acnlapi.azurewebsites.net/api/itemprices/delete?itemName=${itemName}`)
            let newItems = data.filter(item => item.name !== itemName.toLowerCase())
            setData(newItems);
            setFilteredData(newItems);
            setSearchValue('');
            setPrice('');
        }       
    }

    const handleAddClick = () => {
        var isnum = /^\d+$/.test(price);
        if (price && searchValue && isnum) {
        fetch(`https://acnlapi.azurewebsites.net/api/itemprices/add?itemName=${searchValue.toLowerCase().trim()}&price=${price.trim()}`)
        var newItems = [...data];
        newItems.push({ name: searchValue.toLowerCase().trim(), price: Number(price) });
        setData(newItems);
        setFilteredData(newItems);
        setSearchValue('');
        setPrice('');
        }
    }

    return (
        <React.Fragment>
            <Grid stackable columns={2}>
                    <Grid.Column >
                        <Input
                            fluid

                            onChange={handleSearchChange}
                            icon={{ name: 'search' }}
                            placeholder='search'
                            value={searchValue}
                        />
                    </Grid.Column>
                    {filteredData && filteredData.length < 4 &&
                        <React.Fragment>
                            <Grid.Column >
                                <Input
                                    fluid
                                    onChange={handlePriceChange}
                                    value={price}
                                    placeholder='enter price'
                                    action
                                    onClick={handleAddClick}
                                >
                                    <input />
                                    <Button primary icon onClick={handleAddClick}><Icon name='plus' /></Button>

                                    </Input>

                                
                            </Grid.Column>
                        </React.Fragment>
                    }
            </Grid>


            {filteredData && filteredData.length > 0 &&
                <Table sortable unstackable>
                    <Table.Header>
                        <Table.Row>
                            <Table.HeaderCell
                                sorted={column === 'name' ? direction : null}
                                onClick={() => handleSort('name')}
                            >
                                Name
                    </Table.HeaderCell>
                            <Table.HeaderCell
                                sorted={column === 'price' ? direction : null}
                                onClick={() => handleSort('price')}
                            >
                                Price
                    </Table.HeaderCell>
                            <Table.HeaderCell
                            >
                                Actions
                    </Table.HeaderCell>
                        </Table.Row>
                    </Table.Header>
                    <Table.Body>
                        {_.map(filteredData, ({ price, name }) => (
                            <Table.Row key={name}>
                                <Table.Cell>{name}</Table.Cell>
                                <Table.Cell>{Number(price)}</Table.Cell>
                                {!confirmingDeletion && 
                                    <Table.Cell><Button color='red' size='tiny' icon onClick={() => setConfirmingDeletion(true)}> <Icon name='trash' /></Button></Table.Cell>
                                }
                                {confirmingDeletion &&
                                <Table.Cell>
                                    <Button color='red' size='tiny' icon onClick={() => handleDeleteConfirmation(false, name)}> <Icon name='cancel' /></Button>
                                    <Button color='green' size='tiny' icon onClick={() => handleDeleteConfirmation(true, name)}> <Icon name='checkmark' /></Button>
                                    </Table.Cell>
                                }
                            </Table.Row>
                        ))}
                    </Table.Body>
                </Table>
            }
            {filteredData && filteredData.length < 4 &&
            
            <Segment>
                <div>Don't see what you're looking for? Double check your spelling and then add a price!</div>
                </Segment>
            }


</React.Fragment>

    )
}
export default ItemsTable;