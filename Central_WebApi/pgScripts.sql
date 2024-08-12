SELECT "Id", "Usr_Nam", "Pwd", "E_Mail", "RefreshToken" FROM "Users";

select * from "Categories";
select * from "Products";
select * from "ControlMaster";

select * from "Images";

select prd."Product_Name",prd."Price",prd."Quantity_List",prd."IsVeg",prd."IsFeatured",
cat."CategoryName",(SELECT split_part((SELECT "devUrl" FROM "ControlMaster"), ';', 1)) || '/' ||img."Image_Srl" || img."Image_Type" Cat_image,
(SELECT split_part((SELECT "devUrl" FROM "ControlMaster"), ';', 1)) || '/' || imgp."Image_Srl" || imgp."Image_Type" Prd_image from "Categories" cat 
Inner Join "Products" prd on prd."Category_Id" = cat."Category_Id"
Inner Join "Images" img on img."Image_Srl" = cat."Image_Srl"
Inner Join "Images" imgp on imgp."Image_Srl" = prd."Image_Srl";