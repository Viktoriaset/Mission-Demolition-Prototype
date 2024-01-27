using UnityEngine;

public class WoodWall : MonoBehaviour
{
    [SerializeField] private float forceForDestroid = 2f;

    public bool isDestroyed = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile" && !isDestroyed)
        {
            if (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude < forceForDestroid)
                return;

            Vector3 projPos = collision.transform.position;
            float height = transform.localScale.y;
            float startWall = transform.position.y - height / 2;

            float heightDownBlock = projPos.y - startWall;
            float heightUpBlock = height - heightDownBlock;

            CreateBlock(projPos.y - heightDownBlock / 2, heightDownBlock);
            CreateBlock(projPos.y + heightUpBlock / 2, heightUpBlock);

            Destroy(gameObject);
        }
    }

    private void CreateBlock(float posY, float scaleY)
    {
        GameObject block = Instantiate(gameObject);
        block.GetComponent<WoodWall>().isDestroyed = true;
        block.transform.SetParent(transform.parent, false);
        block.transform.position = new Vector3(transform.position.x, posY, transform.position.z);
        block.transform.localScale = new Vector3(transform.localScale.x, scaleY, transform.localScale.z);
    }
}
