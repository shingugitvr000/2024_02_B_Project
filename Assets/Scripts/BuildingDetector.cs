using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDetector : MonoBehaviour
{
    public float checkRadius = 3.0f;                //�ǹ� ���� ����
    private Vector3 lastPosition;                   //�÷��̾��� ������ ��ġ ����
    private float moveThreshold = 0.1f;             //�̵� ���� �Ӱ谪
    private ConstructibleBuilding currentNearbyBuilding;    //���� ������ �ִ� �Ǽ� ������ �ǹ�

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
        CheckForBuilding();
    }

    // Update is called once per frame
    void Update()
    {
        //�÷��̾ ���� �Ÿ� �̻� �̵��ߴ��� üũ 
        if (Vector3.Distance(lastPosition, transform.position) > moveThreshold)
        {
            CheckForBuilding();                                //�̵��� ������ üũ 
            lastPosition = transform.position;              //���� ��ġ�� ������ ��ġ�� ������Ʈ 
        }

        //����� �������� �ְ� E Ű�� ������ �� ������ ���� 
        if (currentNearbyBuilding != null && Input.GetKeyDown(KeyCode.F))
        {
            currentNearbyBuilding.StartConstruction(GetComponent<PlayerInventory>());         //PlayerInventroy�� �����Ͽ� �Ǽ� ���� �Լ� ȣ�� 
        }
    }

    private void CheckForBuilding()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius);  //���� ���� ���� ��� �ݶ��̴��� ã��

        float closestDistance = float.MaxValue;     //���� ����� �Ÿ��� �ʱⰪ
        ConstructibleBuilding closestBuilding = null;         //���� ����� ������ �ʱⰪ

        foreach (Collider collider in hitColliders)   
        {
            ConstructibleBuilding building = collider.GetComponent<ConstructibleBuilding>();       //�ǹ� ����
            if (building != null && building.canBuild && !building.isConstructed) //�ǹ� ���� Ȯ��
            {
                float distance = Vector3.Distance(transform.position, building.transform.position); //�Ÿ� ���
                if (distance < closestDistance)     //�� ����� �������� �߰� �� ������Ʈ 
                {
                    closestDistance = distance;
                    closestBuilding = building;
                }
            }
        }

        if (closestBuilding != currentNearbyBuilding) //���� ����� �������� ����Ǿ��� ���� �޼��� ǥ��
        {
            currentNearbyBuilding = closestBuilding;            //���� ����� ������ ������Ʈ 
            if (currentNearbyBuilding != null)
            {
                if (FloatingTextManager.Instance != null)
                {
                    FloatingTextManager.Instance.Show(
                        $"[F]Ű�� {currentNearbyBuilding.buildingName} �Ǽ� (���� {currentNearbyBuilding.requiredTree} �� �ʿ�)",
                        currentNearbyBuilding.transform.position + Vector3.up
                    );
                }
            }
        }
    }

}
