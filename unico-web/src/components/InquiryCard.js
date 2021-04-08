import {Card, Button} from "react-bootstrap";
import React from "react";
import {MdRemoveRedEye } from "react-icons/md";
import { useHistory } from 'react-router-dom';

export const InquiryCard = ({description, id, createdAt}) => {

    const history = useHistory();
    return (
            <>
                <Card style={{ width: '18rem', margin: '2%' }}>
                    <Card.Body>
                        <Card.Title>inquérito</Card.Title>
                        <Card.Text>
                            {description}
                        </Card.Text>
                        <Card.Text>
                            <p className={"small bold"} variant="red">created at: {createdAt}</p>
                        </Card.Text>
                        <div className={"row justify-content-center"}>
                            <Button variant="primary" className={"space-between" }
                                    onClick={()=>{history.push(`/inquiries/${id}`)}}
                            >

                                Ver Detalhes
                                <MdRemoveRedEye />
                            </Button>
                        </div>
                    </Card.Body>
                </Card></>

    );
};