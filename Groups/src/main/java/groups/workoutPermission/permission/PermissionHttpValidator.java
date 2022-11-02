package groups.workoutPermission.permission;

import groups.common.innerCommunicators.AbstractHttpValidator;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;

import java.util.UUID;

import static groups.common.utils.StringUtils.concatenate;

@Service
public class PermissionHttpValidator extends AbstractHttpValidator implements PermissionValidator {

    private static final String GET_PERMISSION_BY_ID_PATH = "carnets/class-permission/";


    @Override
    public HttpStatus checkPermissionId(UUID permissionId) {

        String path = concatenate(GET_PERMISSION_BY_ID_PATH, permissionId.toString());

        return checkIfIdExist(path);
    }
}