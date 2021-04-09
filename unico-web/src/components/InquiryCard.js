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
                        {id ?
                            <Card.Title>Inquérito</Card.Title>
                            :
                            <Card.Title className={"success"} variant={"info"}>Painel de inquéritos </Card.Title>
                        }
                        <Card.Text>
                            {description}
                        </Card.Text>
                        {createdAt && <Card.Text>
                            <p className={"small bold"} variant="danger">created at: {createdAt}</p>
                        </Card.Text>
                        }
                        {id?
                            <div className={"row justify-content-center"}>
                            <Button variant="info" className={"space-between"}
                                    onClick={() => {
                                        history.push(`/inquiries/${id}`)
                                    }}
                            >
                                Ver Detalhes
                                <MdRemoveRedEye/>
                            </Button>
                        </div>
                            : <></>
                        }
                    </Card.Body>
                </Card></>

    );
};